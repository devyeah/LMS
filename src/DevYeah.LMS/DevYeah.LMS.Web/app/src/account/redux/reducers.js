import {
  AUTHENTICATE_BEGIN,
  AUTHENTICATE_SUCCESS,
  AUTHENTICATE_FAILURE
} 
from "./constants";

export const authenticate = (
  state = {
    isVerified: false, 
    account: null,
    authPending: false,
    authError: null
  }, 
  action
  ) => {
    switch(action.type) {
      case AUTHENTICATE_BEGIN:
        return {
          ...state,
          authPending: true,
        };

      case AUTHENTICATE_SUCCESS:
        return {
          ...state,
          isVerified: true,
          authPending: false,
          account: action.payload
        };

      case AUTHENTICATE_FAILURE:
        return {
          ...state,
          isVerified: false,
          authPending: false,
          authError: action.payload
        };

      default:
        return state;
    }
}
