import React from 'react';
import 'jest-dom/extend-expect';
import { render, fireEvent, cleanup } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import EditProfile from '../EditProfile';

afterEach(cleanup);

const setup = () => {
  const result = render(
    <MemoryRouter>
      <EditProfile />
    </MemoryRouter>
  );
  return result;
};

it('render EditProfile without crashing', () => {
  const { getByLabelText } = setup();
  const fullNameNode = getByLabelText('Full Name');
  expect(fullNameNode.id).toBe('fullName');
});

it('activate form validation', async () => {
  const { getByLabelText, findAllByText } = setup();
  const recoveryEmailNode = getByLabelText('Recovery Email');
  fireEvent.change(recoveryEmailNode, {
    target: {value: "Jack"}
  });
  expect(await findAllByText('Invalid email address')).not.toBeNull();
});