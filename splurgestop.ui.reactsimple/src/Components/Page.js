import React from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { PageTitle } from './PageTitle';

export const Page = ({ title, children }) => (
  <div
    css={css`
      margin: 50px auto 20px auto;
      padding: 30px 20px;
      width: 100%;
      max-width: 1000px;
    `}
  >
    {title && <PageTitle>{title}</PageTitle>}
    {children}
  </div>
);
