import axios from 'axios';

export const createAccount = (signupInfo) => {
  return axios.post("/identity/signup", signupInfo);
}

export const signIn = (signinInfo) => {
  return axios.post("/identity/signin", signinInfo);
}