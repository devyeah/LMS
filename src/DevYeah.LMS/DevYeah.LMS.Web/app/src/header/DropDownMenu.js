import React, { Component } from 'react';
import { Link, withRouter } from 'react-router-dom';
import { connect } from 'react-redux';
import { signout } from '../account/redux/actions';

class DropDownMenu extends Component {

  constructor(props) {
    super(props);

    this.accountSignOut = this.accountSignOut.bind(this);
  }

  accountSignOut() {
    this.props.signout();
    this.history.push("/");
  }

  render() {
    const { account } = this.props;
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
            onClick={this.accountSignOut}
          >
            Log out
          </Link>
        </div>
      </div>
    );
  }
  
}

const mapStateToProps = state => {
  return {
    account: state.authenticate.account
  }
}

export default withRouter(connect(
  mapStateToProps,
  signout
)(DropDownMenu));