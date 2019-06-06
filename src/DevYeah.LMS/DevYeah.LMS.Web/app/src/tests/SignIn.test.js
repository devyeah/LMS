import React from 'react';
import 'jest-dom/extend-expect';
import { render, fireEvent, cleanup } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import SignIn from '../components/SignIn';

afterEach(cleanup);

const setup = () => {
  const result = render(
    <MemoryRouter>
      <SignIn />
    </MemoryRouter>
  );
  return result;
};

it('test SignIn component renders without crashing', () => {
  const { getByLabelText } = setup();
  const emailNode = getByLabelText('Email');
  const passwordNode = getByLabelText('Password');
  expect(emailNode.id).toBe('email');
  expect(passwordNode.id).toBe('password');
});

it('test validation of SignIn form', async () => {
  const { getByLabelText, findAllByText } = setup();
  const emailNode = getByLabelText('Email');
  fireEvent.blur(emailNode);
  expect(emailNode.id).toBe('email');
  expect(await findAllByText('Required!')).not.toBeNull();
});