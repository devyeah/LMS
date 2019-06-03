import React from 'react';
import logo from '../../logo.png';

export default function Navbar() {
  return (
    <nav className="navbar navbar-light bg-light">
      <div className="container">
        <div>
          <a className="navbar-brand font-weight-bold" href="#">
            <img src={logo} width="30" height="30" className="d-inline-block align-top" alt="logo" />
            Dev Yeah!
          </a>
        </div>
        <div id="signButton" >
          <ul className="navbar-nav flex-row">
            <li className="nav-item mr-2">
              <a className="btn btn-outline-primary">Sign In</a>
            </li>
            <li className="nav-item">
              <a className="btn btn-success" href="#">Sign Up</a>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  );
}