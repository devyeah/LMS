import React, { Component } from 'react';
import {formMeta, formValidation} from './formMeta/resetPasswordFormMeta';
import DynamicForm from './DynamicForm';

export default class ResetPassword extends Component {
  constructor(props) {
    super(props);

    this.submitHandler = this.submitHandler.bind(this);
  }

  submitHandler(values, actions) {
    //todo: submit values to backend server
    actions.resetForm();
    actions.setSubmitting(true);
  }

  render() {
    return (
      <DynamicForm 
        formName="resetPasswordForm"
        formMeta={formMeta}
        formValidation={formValidation}
        submitHandler={this.submitHandler} 
      />
    );
  }
}