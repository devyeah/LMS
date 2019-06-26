import axios from 'axios';

export const createAccount = (signupInfo) => {
  return axios.post("https://localhost:44326/api/v1/identity/signup", signupInfo);
}