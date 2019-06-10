import * as yup from 'yup';

const formValidation = yup.object().shape({
  //todo:define a regular expression to validate the specific form that password required
  password: yup.string()
    .min(8, 'Too Short!')
    .max(16, 'Too Long!')
    .required('Required!'),
  confirmPassword: yup.string()
    .oneOf([yup.ref('password')], 'Must match the previous entry')
    .required('Password confirm is required!'),
});

const formMeta = {
  initialValues: {password:'', confirmPassword:''},
  header: {
    id:'setNewPwd',
    alignStyle: null,
    style: 'h4',
    text: 'Reset password',
    tips: {
      id: 'setNewPwdTips',
      style: 'font-weight-normal',
      message: `Your password needs to have at least one symbol or number, 
      and have at least 8 characters.`,
    },
  },
  elements: [
    {
      element: 'input',
      id: 'password',
      label: 'Password',
      type: 'password',
    },
    {
      element: 'input',
      id: 'confirmPassword',
      label: 'Confirm Password',
      type: 'password',
    },
  ],
  button: {
    id: 'resetPasswordBtn',
    text: 'Submit',
    style: 'btn btn-danger btn-block font-weight-bold'
  }
}

export {formMeta, formValidation}