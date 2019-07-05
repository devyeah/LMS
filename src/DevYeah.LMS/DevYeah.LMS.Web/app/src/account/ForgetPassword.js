import React, { Component } from 'react';
import {formMeta, formValidation} from './formMeta/forgetPasswordFormMeta';
import DynamicForm from './DynamicForm';
import * as api from '../common/api';

export default class ForgetPassword extends Component {
  constructor(props){
    super(props);
    this.state = {
      successMsg: null
    }

    this.handleRecoveryPassword = this.handleRecoveryPassword.bind(this);
  }

  handleRecoveryPassword(values, actions) {
    api.recoveryPassword(values.email)
      .then(
        response => {
          this.setState({
            successMsg: response.data || 'An email for recovering your password has been sent.'
          });
          actions.resetForm();
          actions.setSubmitting(false);
        }
      );
  }

  render() {
    return (
      <DynamicForm 
        formName="forgetPasswordForm"
        formMeta={formMeta}
        formValidation={formValidation}
        submitHandler={this.handleRecoveryPassword} 
        success={this.state.successMsg}
      />
    );
  }
}