import React from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { PageTitle } from './PageTitle';

export const Page = ({ title, children }) => (
  <div
    css={css`
      margin: 50px auto 20px auto;
      padding: 30px 20px;
      max-width: 600px;
    `}
  >
    {title && <PageTitle>{title}</PageTitle>}
    {children}
  </div>
);
