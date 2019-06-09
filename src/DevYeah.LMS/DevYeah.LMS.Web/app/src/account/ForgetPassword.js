import React, { Component } from 'react';
import ResetPasswordForm from './ForgetPasswordForm';
import './account.css';

export default class ForgetPassword extends Component {
  constructor(props){
    super(props);

    this.submitHandler = this.submitHandler.bind(this);
  }

  submitHandler(values, actions) {

  }

  render() {
    return (
      <div className="bg-color">
        <ResetPasswordForm submitHandler={this.submitHandler} />
      </div>
    );
  }
}