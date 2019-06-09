import React, { Component } from 'react';
import SignUpForm from './SignUpForm';

export default class SignUp extends Component {
  constructor(props){
    super(props);

    this.submitHandler = this.submitHandler.bind(this);
  }

  submitHandler(values, actions){
    //todo: submit values to backend server
    actions.setSubmitting(false);
    actions.resetForm();
  }

  render(){
    return (
      <div>
        <SignUpForm submitHandler={this.submitHandler} />
      </div>
    );
  }
}