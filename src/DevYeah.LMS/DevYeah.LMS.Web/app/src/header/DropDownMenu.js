import React from 'react';
import { Link } from 'react-router-dom';

export default function DropDownMenu() {
  return (
    <div className="dropdown">
      <button 
        className="btn btn-outline-success dropdown-toggle"
        type="button"
        id="dropDownMenuBtn"
        data-toggle="dropdown"
        aria-haspopup="true" 
        aria-expanded="false"
      >
        Mario
      </button>
      <div 
        className="dropdown-menu" 
        aria-labelledby="dropDownMenuBtn"
      >
        <Link 
          className="dropdown-item" 
          to="/account/editProfile"
          id="profileDropDownMenu"
        >
          Profile
        </Link>
        <Link 
          className="dropdown-item"
          to="/"
          id="logoutMenu"
        >
          Log out
        </Link>
      </div>
    </div>
  );
}