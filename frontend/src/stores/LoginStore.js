import { post } from "axios";
import jwt_decode from "jwt-decode";
import { action, computed, observable } from "mobx";
import moment from "moment";
import { config } from "react-transition-group";

class LoginStore {
  @observable user = null;
  @observable error = false;
  @observable isLoading = false;

  @action async authUser(username, password) {
    const clientid = "sduHealthDroneConnect:sdu201903HealthDrone";
    this.isLoading = true;
    this.error = null;
    try {
      const formData = `username=${encodeURIComponent(username)}&password=${encodeURIComponent(password)}&grant_type=password`;
      const headers = {
        "Accept": "application/json",
        "Content-Type": "application/x-www-form-urlencoded",
        "Authorization": `Basic ${btoa(clientid)}`
      }
      /*const form = new FormData()
      form.append('username', username)
      form.append('password', password)
      form.append('grant_type', 'password')*/
      const response = await post("https://healthdrone.unifly.tech/oauth/token", formData, {
        headers: headers
      });
      this.user = response.data
      localStorage.setItem("authorization", response.data.access_token);
      this.isLoading = false;
    } catch (err) {
      this.error = err;
      this.isLoading = false;
    }
  }

  @computed get isLoggedIn() {
    return !!this.user;
  }

  @action setError(value) {
    this.error = value;
  }

  isLoggedInPersist() {
    const token = localStorage.getItem("authorization");
    if (token == null) return false;
    return jwt_decode(token).exp > moment().unix();
  }

  @action
  logout() {
    localStorage.removeItem("authorization");
    this.user = null;
  }
}

const loginStore = new LoginStore();
export default loginStore;