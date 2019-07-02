import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import { signout } from '../account/redux/actions';

function DropDownMenu({ signout, account }) {

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
        <span id="navbarUserMenu">
          { `Welcome, ${account.username}` }
        </span>
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
          id="logoutMenu"
          to="/"
          onClick={() => (signout())}
        >
          Log out
        </Link>
      </div>
    </div>
  );
}

const mapStateToProps = state => {
  return {
    account: state.authenticate.account
  }
}

export default connect(
  mapStateToProps,
  {signout}
)(DropDownMenu);