import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { userReducer } from "./store/reducers/user";
//import thunk from "redux-thunk";
import { applyMiddleware, combineReducers, createStore } from "redux";
import { Provider } from 'react-redux';

 const store = createStore(userReducer);//, applyMiddleware(thunk));

ReactDOM.render(
  <React.StrictMode>
  <Provider store={store}>
    <App />
  </Provider>  
  </React.StrictMode>,
  document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
