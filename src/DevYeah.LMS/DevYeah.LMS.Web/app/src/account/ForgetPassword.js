import React, { Component } from 'react';
import {formMeta, formValidation} from './formMeta/forgetPasswordFormMeta';
import DynamicForm from './DynamicForm';

export default class ForgetPassword extends Component {
  constructor(props){
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
        formName="forgetPasswordForm"
        formMeta={formMeta}
        formValidation={formValidation}
        submitHandler={this.submitHandler} 
      />
    );
  }
}