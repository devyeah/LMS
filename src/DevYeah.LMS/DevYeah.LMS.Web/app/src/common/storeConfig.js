import { createStore, applyMiddleware, combineReducers, compose  } from 'redux';
import { persistStore, persistReducer } from 'redux-persist';
import storage from 'redux-persist/lib/storage';
import autoMergeLevel2 from 'redux-persist/lib/stateReconciler/autoMergeLevel2';
import thunk from 'redux-thunk';
import { authenticate } from '../account/redux/reducers';
import { category } from '../category/redux/reducers';

const storeConfig = {
  key: 'root',
  storage: storage,
  stateReconciler: autoMergeLevel2
}

const configureStore = () => {
  const middlewares = [thunk];
  const rawReducer = combineReducers({
    authenticate,
    category
  });
  const rootReducer = persistReducer(storeConfig, rawReducer);
  const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;
  const store = createStore(
    rootReducer,
    composeEnhancers(applyMiddleware(...middlewares))
  );
  const persistor = persistStore(store);
  
  return { store, persistor };
}

export default configureStore;