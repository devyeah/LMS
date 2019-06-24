import { createStore, combineReducers  } from 'redux';
import throttle from 'lodash/throttle';
import { authenticate } from '../account/redux/reducers';

const loadState = () => {
  try {
    const serializedState = localStorage.getItem('state');
    if (serializedState === null) {
      return undefined; // let redux to initialize the state of store
    }
    return JSON.parse(serializedState);
  } catch (error) {
    return undefined;  // let redux to initialize the state of store
  }
}

const saveState = (state) => {
  try {
    const serializedState = JSON.stringify(state);
    localStorage.setItem('state', serializedState);
  } catch (error) {
    // log errors
  }
}

const configureStore = () => {
  const rootReducer = combineReducers({
    authenticate
  });
  
  const persistedState = loadState();
  const store = createStore(
    rootReducer,
    persistedState
    );
  
  store.subscribe(throttle(() => {
    saveState({
      authenticate: store.getState().authenticate
    });
  }, 1000)); // calling once per second

  return store;
}

export default configureStore;