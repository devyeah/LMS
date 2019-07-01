import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import AuthenticationButtons from './AuthenticationButtons';
import DropDownMenu from './DropDownMenu';
import logo from '../images/logo.png';
import './header.css';

function Navbar({ isLoggedIn }) {
  return (
    <nav className="navbar navbar-light bg-light">
      <div className="container">
        <div>
          <Link className="navbar-brand font-weight-bold" to="/">
            <img id="logo" src={logo} className="d-inline-block align-top" alt="logo" />
            Dev Yeah!
          </Link>
        </div>
        <div id="navbarAction" >
          { isLoggedIn ? <DropDownMenu /> : <AuthenticationButtons /> }
        </div>
      </div>
    </nav>
  );
}

const mapStateToProps = state => {
  return {
    isLoggedIn : state.authenticate.isVerified
  }
}

export default connect(
  mapStateToProps,
  null
)(Navbar);