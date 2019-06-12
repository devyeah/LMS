import React, { Component } from 'react';
import DynamicForm from './DynamicForm';
import { formMeta, formValidation} from './formMeta/profileFormMeta';

export default class EditProfile extends Component {
  constructor(props) {
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
      <div className="card">
        <div className="card-header">
          Profile details
        </div>
        <div className="card-body">
          <DynamicForm 
            isEmbedded
            formName="profileForm"
            submitHandler={this.submitHandler}
            formValidation={formValidation}
            formMeta={formMeta}
          />
        </div>
      </div>
    );
  }
  
}