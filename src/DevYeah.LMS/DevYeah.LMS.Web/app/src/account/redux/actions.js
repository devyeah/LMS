import {
  ACCOUNT_AUTH_REQUEST,
  ACCOUNT_AUTH_SUCCESS,
  ACCOUNT_AUTH_ERROR
} 
from './constants';
import { createAccount } from '../../common/api';

export const signup = (signupInfo) => (dispatch) => {
  dispatch({ 
    type: ACCOUNT_AUTH_REQUEST 
  });
  
  return createAccount(signupInfo).then(
    response => {
      dispatch({
        type: ACCOUNT_AUTH_SUCCESS,
        payload: response
      })
    },
    error => {
      dispatch({
        type: ACCOUNT_AUTH_ERROR,
        payload: error.message || 'Something went wrong.'
      })
    }
  );
}