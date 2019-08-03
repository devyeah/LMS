import React from 'react';
import { Link } from 'react-router-dom';

export default function AuthenticationButtons() {
  return (
    <ul className="navbar-nav flex-row">
      <li className="nav-item">
        <Link 
          to="/signup"
          id="signUpBtn" 
          className="btn btn-success font-weight-bold"
        >
          Sign Up
        </Link>
      </li>
      <li className="nav-item mr-2">
        <Link 
          to="/signin" 
          id="signInBtn" 
          className="btn btn-link font-weight-bold"
        >
          Sign In
        </Link>
      </li>
    </ul>
  );
}