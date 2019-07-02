import React from 'react';
import ReactDOM from 'react-dom';
import axios from 'axios';

import Root from './Root';
import './index.css';
import * as serviceWorker from './serviceWorker';

axios.defaults.baseURL = "https://localhost:5001/api/v1";

ReactDOM.render(
  <Root />, 
  document.getElementById('root')
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
