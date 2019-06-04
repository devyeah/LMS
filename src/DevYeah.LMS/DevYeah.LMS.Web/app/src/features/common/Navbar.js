import React from 'react';
import { Link } from 'react-router-dom';
import logo from '../../logo.png';
import './header.css';

export default function Navbar() {
  return (
    <nav className="navbar navbar-light bg-light">
      <div className="container">
        <div>
          <Link className="navbar-brand font-weight-bold" to="/">
            <img id="logo" src={logo} className="d-inline-block align-top" alt="logo" />
            Dev Yeah!
          </Link>
        </div>
        <div id="signButton" >
          <ul className="navbar-nav flex-row">
            <li className="nav-item mr-2">
              <a id="signInBtn" className="btn btn-outline-primary">Sign In</a>
            </li>
            <li className="nav-item">
              <Link 
                to="/signup"
                id="signUpBtn" 
                className="btn btn-success"
              >
                Sign Up
              </Link>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  );
}