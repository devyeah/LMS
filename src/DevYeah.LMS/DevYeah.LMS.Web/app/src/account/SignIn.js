import React, { Component } from 'react';
import {formMeta, formValidation} from './formMeta/signInFormMeta';
import DynamicForm from './DynamicForm';

export default class SignIn extends Component {
  constructor(props){
    super(props);

    this.submitHandler = this.submitHandler.bind(this);
  }

  submitHandler(values, actions) {
    //todo: submit values to backend server
    actions.setSubmitting(false);
    actions.resetForm();
  }

  render() {
    return (
      <DynamicForm 
        formName="signInForm"
        formMeta={formMeta}
        formValidation={formValidation}
        submitHandler={this.submitHandler} 
      />
    );
  }
}