import React, { Component } from 'react';
import CardHeader from './CardHeader';
import * as api from '../common/api';

const validHeaderProps = {
  id:'validActiveAccountHeader',
  alignStyle: null,
  style: 'h4',
  text: 'Active Your Account',
  tips: {
    id: 'validActiveAccountTips',
    style: 'font-weight-normal',
    message: `Welcom to DevYeah! In order to started, you need to active your account.`,
  },
};

export default class ActiveAccount extends Component {
  
  constructor(props) {
    super(props);
    this.state = {
      isActiveSuccess: false,
      isTriggered: false,
      isTouched: false
    };

    this.handleActiveAccount = this.handleActiveAccount.bind(this);
  }

  handleActiveAccount() {
    this.setState({
      isTouched: true
    });
    const token = this.props.match.params.token;
    api.activeAccount({token: token})
      .then(
        response => {
          this.setState({
            isActiveSuccess: true,
            isTriggered: true
          });
        },
        error => {
          this.setState({
            isTriggered: true
          });
        }
      );
  }

  render() {

    return (
      <div className="row align-items-center h-100">
        <div className="col-6 mx-auto">
          <div className="card form-center h-100 justify-content-center">
            <CardHeader 
              header={validHeaderProps}
            />  
            {
              this.state.isTriggered
              ? (
                this.state.isActiveSuccess 
                ? (
                  <div className="alert alert-success" role="alert">
                    <span
                      id="successActiveAccountMsg"
                    >
                      Your account has been activated successfully.
                    </span>
                  </div>
                )
                : (
                  <div className="alert alert-danger" role="alert">
                    <span
                      id="failureActiveAccountMsg"
                    >
                      The activation to your account is failed.
                    </span>
                  </div>
                )
                
              ):(
                <button
                  type="button" 
                  className="btn btn-danger btn-block font-weight-bold" 
                  id="activeAccountBtn"
                  disabled={this.state.isTouched ? true : false}
                  onClick={this.handleActiveAccount}
                > 
                  Active Account
                </button>
              )
            }
          </div>
        </div>
      </div>
    );
  }
  
}