import { createStore, applyMiddleware, combineReducers, compose  } from 'redux';
import thunk from 'redux-thunk';
import { authenticate } from '../account/redux/reducers';

const configureStore = () => {
  const middlewares = [thunk];
  const rootReducer = combineReducers({
    authenticate
  });
  const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;
  const store = createStore(
    rootReducer,
    composeEnhancers(applyMiddleware(...middlewares))
    );
  
  return store;
}

export default configureStore;