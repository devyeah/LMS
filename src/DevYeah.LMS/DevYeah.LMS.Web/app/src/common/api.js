import axios from 'axios';

export const createAccount = (signupInfo) => {
  return axios.post("/identity/register", signupInfo);
}

export const signIn = (signinInfo) => {
  return axios.post("/identity/login", signinInfo);
}

export const activeAccount = (token) => {
  return axios.post(
    "/identity/active", 
    null,
    { params: {token} }
  );
}

export const recoveryPassword = (email) => {
  return axios.post(
    "/identity/recoverypassword", 
    null,
    { params: {email}} 
  );
}

export const resetPassword = (resetPwdInfo) => {
  return axios.post("/identity/updatepassword", resetPwdInfo);
}