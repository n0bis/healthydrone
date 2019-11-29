import { get, action, observable, toJS } from "mobx";
import axios from "axios";

class ReportStore {
    @observable isModalOpen = false;

    @action onOpenModal = () => { 
        this.isModalOpen = true;
    };
    @action onCloseModal = () => { 
        this.isModalOpen = false;
    };
    @action onSendModal = e => {
      console.log(e.target.value)
     
        this.isModalOpen = false;
    }
   
}

const reportStore = new ReportStore();
export default reportStore;