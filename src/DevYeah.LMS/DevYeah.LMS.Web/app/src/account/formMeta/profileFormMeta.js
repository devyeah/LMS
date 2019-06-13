import * as yup from 'yup';

const formMeta = {
  initialValues: {
    recoveryEmail: '',
    fullName: '',
    gender: '',
    birthDay: '',
    introduction: ''
  },
  elements: [
    {
      element: 'input',
      id: 'recoveryEmail',
      label: 'Recovery Email',
      type: 'email',
    },
    {
      element: 'input',
      id: 'fullName',
      label: 'Full Name',
      type: 'text',
    },
    {
      element: 'select',
      id: 'gender',
      label: 'I Am',
      options: [
        {order: 0, value: '', text: '(Gender)'},
        {order: 1, value: 'M', text: 'Male'},
        {order: 2, value: 'F', text: 'Female'},
        {order: 3, value: 'O', text: 'Other'},
      ]
    },
    {
      element: 'input',
      id: 'birthDay',
      label: 'Birth Day',
      type: 'text',
    },
    {
      element: 'textarea',
      id: 'introduction',
      label: 'Describe Yourself',
      rows: 5,
    }
  ],
  button: {
    id: 'saveProfileBtn',
    text: 'Save',
    style: 'btn btn-danger font-weight-bold'
  }
}

const formValidation = yup.object().shape({
  fullName: yup.string()
    .min(3, 'Too Short!')
    .max(50, 'Too Long!'),
  recoveryEmail: yup.string()
    .email('Invalid email address'),
  birthDay: yup.date(),
  introduction: yup.string()
    .max(300, 'Too Long!'),
});

export {formMeta, formValidation}