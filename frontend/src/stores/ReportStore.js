import { get, action, observable, toJS } from "mobx";
import axios from "axios";

class ReportStore {
  @observable isModalOpen = false;
  @observable data = {};

  @action onOpenModal = () => {
    this.isModalOpen = true;
  };
  @action onCloseModal = () => {
    this.isModalOpen = false;
  };

  @action handleChange = e => {
    this.data[e.target.name] = e.target.value;
  };

  @action onSendModal = e => {
    axios
      .post("/INSERT_BACK_END_URL", this.data)
      .then(res => {
        console.log(res);
      })
      .catch(err => {
        console.log(err);
      });
    this.isModalOpen = false;
  };
}

const reportStore = new ReportStore();
export default reportStore;
