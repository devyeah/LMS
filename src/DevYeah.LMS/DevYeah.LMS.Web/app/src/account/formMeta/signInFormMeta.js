import * as yup from 'yup';

const formValidation = yup.object().shape({
  email: yup.string()
    .email('Invalid email address')
    .required('Required!'),
  password: yup.string()
    .min(8, 'Too Short!')
    .max(16, 'Too Long!')
    .required('Required!'),
});

const formMeta = {
  initialValues: { email: '', password: '' },  
  header: {
    id:'title',
    alignStyle: 'text-center',
    style: 'h2',
    text: 'Dev Yeah!',
    tips: null,
  },
  elements: [
    {
      element: 'input',
      id: 'email',
      label: 'Email',
      type: 'email',
      placeholder: 'Email address'
    },
    {
      element: 'input',
      id: 'password',
      label: 'Password',
      type: 'password',
      placeholder: 'Password',
    },
  ],
  button: {
    id: 'signinBtn',
    text: 'Sign In',
    style: 'btn btn-danger btn-block font-weight-bold'
  },
  extraLink: {
    id: 'passwordForget',
    target: '/forgetPassword',
    text: 'Forget Password?',
  }  
}

export {formMeta, formValidation}