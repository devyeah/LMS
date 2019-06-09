import React, { Component } from 'react';
import ResetPasswordForm from './ResetPasswordForm';

export default class ResetPassword extends Component {
  constructor(props) {
    super(props);

    this.submitHandler = this.submitHandler.bind(this);
  }

  submitHandler(values, actions) {
    //todo: submit values to backend server
    console.log(values);
    actions.setSubmitting(false);
    actions.resetForm();
  }

  render() {
    return (
      <ResetPasswordForm submitHandler={this.submitHandler} />
    );
  }
}