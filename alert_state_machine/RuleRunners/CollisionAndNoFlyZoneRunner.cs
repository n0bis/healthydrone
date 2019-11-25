using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using alert_state_machine.Persistence;
using alert_state_machine.Services;
using AutoMapper;
using Newtonsoft.Json;
using utm_service;
using utm_service.Models;

namespace alert_state_machine.RuleRunners
{
    public class CollisionAndNoFlyZoneRunner : ICollisionAndNoFlyZoneRunner
    {
        private readonly IRedisService _redisService;
        private Boolean isConnected = false;
        private readonly IUTMLiveService _UTMLiveService;
        private object _ObjectData;

        public CollisionAndNoFlyZoneRunner(IRedisService redisService, IUTMLiveService utmLiveService)
        {
            _redisService = redisService;
            _redisService.Connect();
            _UTMLiveService = utmLiveService;
        }

        public async Task ZonesCheck(Token token)
        {
            if (isConnected == false)
            {
                var action = new Action<object, string, object>(OnMessage);

                await _UTMLiveService.Connect(token?.access_token, action);
                isConnected = true;

                
            }



        }

        private object OnMessage(object sender, string name, object data)
        {
            //Console.WriteLine($"{((PureSocketClusterSocket)sender).InstanceName} {name} : {data} \r\n", ConsoleColor.Green);
            //Console.WriteLine(data);


            var test = (Dictionary<string, object>)(data);
            if (test.Values.ElementAt(1).ToString() == "ADSB_TRACK")
                return;

            try
            {

                var testa = (IEnumerable<object>)test.Values.FirstOrDefault();

                var result = JsonConvert.SerializeObject(DictionaryToObject(testa), Formatting.Indented);
                var obj = JsonConvert.DeserializeObject<Models.Message>(result);

                Console.WriteLine(obj);
            }
            catch (Exception e)
            {

            }

        }

        private static dynamic DictionaryToObject(IEnumerable<object> dictionary)
        {
            var expandoObj = new ExpandoObject();
            var expandoObjCollection = (ICollection<KeyValuePair<String, Object>>)expandoObj;

            foreach (var keyValuePair in dictionary)
            {
                var test = (Dictionary<string, object>)keyValuePair;
                foreach (var v in test)
                {
                    expandoObjCollection.Add(v);
                }
            }
            dynamic eoDynamic = expandoObj;
            return eoDynamic;
        }
    }
}
