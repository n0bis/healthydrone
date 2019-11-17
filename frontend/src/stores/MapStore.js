import { get, action, observable, toJS } from "mobx";
import axios from "axios";
import { observer } from "mobx-react-lite";

class MapStore {
  @observable isLoading = false;
  @observable landing = null;
  @observable dronePos = [-122.36777130126949, 37.76914513791027];
  @observable b;
  @observable data = {
    type: "FeatureCollection",
    features: [
      {
        type: "Feature",
        properties: {
          name: "Operation-Name",
          minHeight: 0,
          maxHeight: "100",
          startTime: "{{DATE_START}}",
          endTime: "{{DATE_END}}",
          uas: "{{UAS_UUID}}",
          pilotMobile: "{{USER_MOBILE}}",
          pilotUuid: "{{USER_COMPANY_UUID}}",
          crew: {
            contact: {
              mobile: "{{USER_MOBILE}}"
            },
            pilot: "{{USER_COMPANY_UUID}}"
          },
          lineOfSightType: "B_VLOS",
          rulesetCode: "Recreational",
          environment: "UNPOPULATED",
          crowdRisk: false,
          obstacleRisk: false,
          takeOffPosition: {
            latitude: 55.470938,
            longitude: 10.326246
          },
          landPosition: {
            latitude: 55.470855,
            longitude: 10.3261
          },
          originalGeometry: {
            type: "GeoBuffer",
            buffer: 50,
            baseshape: {
              type: "LineString",
              coordinates: [
                [10.326391813947383, 55.47074433454168],
                [10.31952631650976, 55.47159991406579],
                [10.311054000522907, 55.46881231247039],
                [10.308473352549898, 55.47085473294308],
                [10.322106964482769, 55.47535320480927],
                [10.326537888360951, 55.47077193417101]
              ]
            },
            properties: {
              buffer_units: "m"
            }
          }
        },
        geometry: {
          type: "Polygon",
          coordinates: [
            [
              [10.32622236137656, 55.470305665097605],
              [10.326376270531838, 55.4702953148556],
              [10.326530776991762, 55.47030221999475],
              [10.326679943356854, 55.47032611516321],
              [10.326715679318784, 55.47033645814659],
              [10.326776042167667, 55.47034368819053],
              [10.326918539960403, 55.47037830938067],
              [10.32704640995734, 55.47042805722394],
              [10.32715473825833, 55.470491019991144],
              [10.327239361862945, 55.470564778114834],
              [10.32729702865296, 55.47064649716316],
              [10.327325522381491, 55.47073303675783],
              [10.32732374786454, 55.47082107125108],
              [10.327291773096192, 55.47090721752535],
              [10.327230826666346, 55.47098816500491],
              [10.325015521247789, 55.473278833262675],
              [10.32279995889835, 55.47556946055844],
              [10.322710851914895, 55.475643114736975],
              [10.322597832648933, 55.475705289502486],
              [10.322465376262203, 55.47575352289978],
              [10.322318727636747, 55.47578590500693],
              [10.322163693682668, 55.47580115356957],
              [10.322006413381583, 55.47579866477986],
              [10.321853114674198, 55.47577853718817],
              [10.321709867823463, 55.475741567800355],
              [10.314892629310789, 55.47349250009225],
              [10.308076167088286, 55.47124305199828],
              [10.307943340462073, 55.471187976587025],
              [10.307833445058758, 55.471118483519916],
              [10.307751235410729, 55.471037579439],
              [10.30770026814968, 55.470948764668336],
              [10.307682748151079, 55.47085588176454],
              [10.307699433174678, 55.470762949264326],
              [10.307749601120026, 55.470673987822344],
              [10.307831081307826, 55.470592846262356],
              [10.309121446696556, 55.4695716496434],
              [10.310411745377255, 55.46855043923988],
              [10.310507022415571, 55.468488053758605],
              [10.310620798194774, 55.468436634890274],
              [10.31074922477364, 55.46839792161248],
              [10.310887958765923, 55.46837322319739],
              [10.311032308224155, 55.46836337493652],
              [10.311177391306746, 55.468368709894],
              [10.311318301363615, 55.46838904764387],
              [10.311450272860915, 55.468423700371034],
              [10.315686253720814, 55.46981756100136],
              [10.319656835819385, 55.47112387051219],
              [10.322789686110335, 55.47073349828033],
              [10.32622236137656, 55.470305665097605]
            ],
            [
              [10.325056959355695, 55.47137050742249],
              [10.323128522451846, 55.47161084634198],
              [10.319695700277911, 55.47203859268241],
              [10.319550586884526, 55.472048808802974],
              [10.31940464329837, 55.4720436703685],
              [10.319262861666246, 55.47202335314426],
              [10.319130091757248, 55.47198855210061],
              [10.314893756618435, 55.470594811134376],
              [10.311278170412743, 55.46940509954937],
              [10.310405981595666, 55.470095406176455],
              [10.309623287617088, 55.47071485490008],
              [10.315686903643138, 55.47271581673181],
              [10.3218038729937, 55.47473387666525],
              [10.323629596113737, 55.47284634275242],
              [10.325056959355695, 55.47137050742249]
            ]
          ]
        }
      }
    ]
  };
  @observable landingSpots = [
    {
      id: 1,
      name: "Svendbord",
      value: [-55.676098, 12.568337]
    },
    {
      id: 2,
      name: "København",
      value: [55.676098, 2.568337]
    }
  ];

  @action onChange = data => {
    this.data = data;
  };

  @action setLanding = e => {
    const landing = this.landingSpots.filter(item => {
      return e.target.value == item.id;
    });

    //if (!landing[0]) return (this.landing = landingSpots[0].value);

    this.landing = landing[0].value;
    console.log(this.landing);
  };

  @action startFligth = () => {
    alert("Flight started");
  };

  @action createFlight = (a, b) => {
    /**/
    this.isLoading = true;

    var data = toJS(this.data);

    data.features.push({
      type: "Feature",
      properties: {},
      geometry: {
        coordinates: [this.landing, [-122.36777130126949, 37.76914513791027]],
        type: "LineString"
      }
    });

    this.data = data;
    this.isLoading = false;

    //this.data.features = test;

    /*
    this.data.features.push({
      type: "Feature",
      properties: {},
      geometry: {
        coordinates: [
          [55.676098, 12.568337],
          [-122.36777130126949, 37.76914513791027]
        ],
        type: "LineString"
      }
    });*/
    //console.log(toJS(this.data));
  };
}

const mapStore = new MapStore();
export default mapStore;