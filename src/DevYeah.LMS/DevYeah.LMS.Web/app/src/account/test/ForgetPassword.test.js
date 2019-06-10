import React from 'react';
import 'jest-dom/extend-expect';
import { render, fireEvent, cleanup, findAllByText } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import ForgetPassword from '../ForgetPassword';

afterEach(cleanup);

const setup = () => {
  const result = render(
    <MemoryRouter>
      <ForgetPassword />
    </MemoryRouter>
  );
  return result;
};

it('render without crashing', () => {
  const { getByLabelText } = setup();
  const emailNode = getByLabelText('Email address');
  expect(emailNode.id).toBe('email');
});

it('activate validation with blur event', async () => {
  const { getByLabelText, findAllByText } = setup();
  const emailNode = getByLabelText('Email address');
  fireEvent.blur(emailNode);
  expect(await findAllByText('Required!')).not.toBeNull();
});