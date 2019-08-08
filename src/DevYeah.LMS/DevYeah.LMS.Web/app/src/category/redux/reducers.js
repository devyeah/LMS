import { SWITCH_CATEGORY } from './constants';

export const category = (
  state = {
    activeCategory: null
  },
  action
) => {
  switch(action.type) {
    case SWITCH_CATEGORY:
      return {
        ...state,
        activeCategory: action.payload
      };
    default:
      return state;
  }
}