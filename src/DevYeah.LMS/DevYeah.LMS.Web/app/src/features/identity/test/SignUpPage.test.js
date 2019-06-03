import React from 'react';
import 'jest-dom/extend-expect';
import { render, fireEvent, cleanup } from '@testing-library/react';
import SignUpPage from '../SignUpPage';

afterEach(cleanup);

const setup = () => {
  const result = render(<SignUpPage />);
  return result;
};

test('test SignUpPage component renderring', () => {
  const {getByLabelText} = setup();
  const usernameNode = getByLabelText('Username');
  const emailNode = getByLabelText('Email');
  const passwordNode = getByLabelText('Password');
  const usertypeNode = getByLabelText('User Type');
  expect(usernameNode).toHaveAttribute('id', 'username');
  expect(emailNode).toHaveAttribute('id', 'email');
  expect(passwordNode).toHaveAttribute('id', 'password');
  expect(usertypeNode).toHaveAttribute('id', 'userType');
});

it('test input error help message', async () => {
  const {getByPlaceholderText, findAllByText} = setup();
  const emailNode = getByPlaceholderText('Email Address');
  fireEvent.blur(emailNode);
  expect(emailNode.id).toBe('email');
  expect(await findAllByText('Required!')).not.toBeNull();
});
