import React, { Component } from 'react';
import {formMeta, formValidation} from './formMeta/resetPasswordFormMeta';
import DynamicForm from './DynamicForm';
import * as api from '../common/api';

export default class ResetPassword extends Component {
  constructor(props) {
    super(props);
    this.state = {
      errorMsg: null,
      successMsg: null
    }

    this.handleResetPassword = this.handleResetPassword.bind(this);
    this.resetForm = this.resetForm.bind(this);
  }

  handleResetPassword(values, actions) {
    const token = this.props.match.params.token;
    api.resetPassword({
      token,
      newPassword: values.password
    })
    .then(
      response => {
        this.setState({
          successMsg: "Your password has been successfully resetted."
        });
        this.resetForm(actions);
      }
    )
    .catch(
      error => {
        this.setState({
          errorMsg: error.request.responseText
        });
        this.resetForm(actions);
      }
    );
  }

  resetForm(actions) {
    actions.resetForm();
    actions.setSubmitting(false);
  }

  render() {
    const { errorMsg, successMsg } = this.state;
    return (
      <DynamicForm 
        formName="resetPasswordForm"
        formMeta={formMeta}
        formValidation={formValidation}
        submitHandler={this.handleResetPassword} 
        error={errorMsg}
        success={successMsg}
      />
    );
  }
}