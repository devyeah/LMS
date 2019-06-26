import { createStore, applyMiddleware, combineReducers  } from 'redux';
import thunk from 'redux-thunk';
import { authenticate } from '../account/redux/reducers';

const configureStore = () => {
  const middlewares = [thunk];
  const rootReducer = combineReducers({
    authenticate
  });
  
  const store = createStore(
    rootReducer,
    applyMiddleware(...middlewares)
    );

  return store;
}

export default configureStore;