import React, { Component } from 'react';
import { formMeta, formValidation} from './formMeta/signUpFormMeta';
import DynamicForm from './DynamicForm';

export default class SignUp extends Component {
  constructor(props){
    super(props);

    this.submitHandler = this.submitHandler.bind(this);
  }

  submitHandler(values, actions){
    //todo: submit values to backend server
    console.log(values);
    actions.setSubmitting(false);
    actions.resetForm();
  }

  render(){
    return (
      <DynamicForm 
        formName="signUpForm"
        submitHandler={this.submitHandler} 
        formValidation={formValidation}
        formMeta={formMeta}
      />
    );
  }
}