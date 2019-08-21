import { SWITCH_CATEGORY } from './constants';

export function switchCategory(catId) {
  return {
    type: SWITCH_CATEGORY,
    payload: catId
  }
}