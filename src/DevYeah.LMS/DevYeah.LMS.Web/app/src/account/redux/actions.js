import {
  ACCOUNT_AUTH_REQUEST,
  ACCOUNT_AUTH_SUCCESS,
  ACCOUNT_AUTH_ERROR,
  ACCOUNT_SIGNOUT
} 
from './constants';
import * as api from '../../common/api';

export function signup(signupInfo) { 
  return function(dispatch) {
    dispatch({ 
      type: ACCOUNT_AUTH_REQUEST 
    });
    
    return api.createAccount(signupInfo)
      .then(
        response => {
          dispatch({
            type: ACCOUNT_AUTH_SUCCESS,
            payload: response.data
          })
        },
        error => {
          dispatch({
            type: ACCOUNT_AUTH_ERROR,
            payload: error.message || 'Something went wrong.'
          })
          return error;
        }
      );
  }
}

export function signin(signinInfo) {
  return function(dispatch) {
    dispatch({ 
      type: ACCOUNT_AUTH_REQUEST 
    });

    return api.signIn(signinInfo)
      .then(
        response => {
          dispatch({
            type: ACCOUNT_AUTH_SUCCESS,
            payload: response.data
          })
        },
        error => {
          dispatch({
            type: ACCOUNT_AUTH_ERROR,
            payload: error.message || 'Something went wrong.'
          })
          return error;
        }
      );
  }  
}

export function signout() {
  return {
    type: ACCOUNT_SIGNOUT
  }
}