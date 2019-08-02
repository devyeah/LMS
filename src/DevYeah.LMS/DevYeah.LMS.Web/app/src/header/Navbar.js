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
      <div className="d-inline-flex w-100 justify-content-between px-5">
        <div className="py-2">
          <Link className="navbar-brand font-weight-bold" to="/">
            <img id="logo" src={logo} className="d-inline-block align-top" alt="logo" />
            Dev Yeah!
          </Link>
        </div>
        <div id="navbarAction" className="d-inline-flex justify-content-between py-2 align-items-center" >
          <form className="form-inline px-3">
            <input 
              id="globalSerch"
              className="form-control mr-sm-2" 
              type="search"
              placeholder="Search"
              aria-label="Search"
            />
            <button
              id="globalSearchBtn"
              className="btn btn-outline-success my-2 my-sm-0"
              type="submit"
            >
              Search
            </button>
          </form>
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