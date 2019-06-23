export const authenticate = (
  state = {isVerified: false}, 
  action
  ) => {
    switch(action.type) {
      case 'AUTHENTICATE_BEGIN':
        return {
          ...state,
          authPending: true,
          authError: null
        };

      case 'AUTHENTICATE_SUCCESS':
        return {
          isVerified: true,
          authPending: false,
          ...state
        };

      case 'AUTHENTICATE_FAILURE':
        return {
          isVerified: false,
          authPending: false,
          authError: action.payload
        };

      default:
        return state;
    }
}
