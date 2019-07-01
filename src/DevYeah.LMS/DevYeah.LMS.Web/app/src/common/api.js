import axios from 'axios';

export const createAccount = (signupInfo) => {
  return axios.post("/identity/register", signupInfo);
}

export const signIn = (signinInfo) => {
  return axios.post("/identity/login", signinInfo);
}