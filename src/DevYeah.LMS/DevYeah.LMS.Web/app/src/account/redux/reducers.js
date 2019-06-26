import {
  ACCOUNT_AUTH_REQUEST,
  ACCOUNT_AUTH_SUCCESS,
  ACCOUNT_AUTH_ERROR
} 
from './constants';

export const authenticate = (
  state = {
    isVerified: false, 
    account: undefined,
    isPending: false,
    errorMessage: undefined
  }, 
  action
  ) => {
    switch(action.type) {
      case ACCOUNT_AUTH_REQUEST:
        return {
          ...state,
          isPending: true,
        };

      case ACCOUNT_AUTH_SUCCESS:
        return {
          ...state,
          isVerified: true,
          isPending: false,
          account: action.payload
        };

      case ACCOUNT_AUTH_ERROR:
        return {
          ...state,
          isVerified: false,
          isPending: false,
          errorMessage: action.payload
        };

      default:
        return state;
    }
}
