/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { fontFamily, fontSize, gray1, gray2, gray5 } from '../Styles';
import './Header.css';

export const Header = () => (
  <div
    css={css`
      position: fixed;
      box-sizing: border-box;
      top: 0;
      width: 100%;
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 10px 20px;
      background-color: #fff;
      border-bottom: 1px solid ${gray5};
      box-shadow: 0 3px 7px 0 rgba(110, 112, 114, 0.21);
    `}
  >
    <a
      href="/"
      css={css`
        font-size: 24px;
        font-weight: bold;
        color: ${gray1};
        text-decoration: none;
      `}
    >
      Splurge Stop!
    </a>
    <div className="dropdown">
      <button className="dropbtn">Purchase transactions</button>
      <div className="dropdown-content">
        <a href="/">Purchase transactions</a>
        <a href="/Currency">Currencies</a>
      </div>
    </div>
    <div className="dropdown">
      <button className="dropbtn">Stores</button>
      <div className="dropdown-content">
        <a href="/Store">Stores</a>
        <a href="/Location">Locations</a>
        <a href="/City">Cities</a>
        <a href="/Country">Countries</a>
      </div>
    </div>
    <div className="dropdown">
      <button className="dropbtn">Products</button>
      <div className="dropdown-content">
        <a href="/Product">Products</a>
        {/* size */}
        <a href="/Brand">Brands</a>
        <a href="/ProductType">Product types</a>
      </div>
    </div>
    <input
      type="text"
      placeholder="Search..."
      css={css`
        box-sizing: border-box;
        font-family: ${fontFamily};
        font-size: ${fontSize};
        padding: 8px 10px;
        border: 1px solid ${gray5};
        border-radius: 3px;
        color: ${gray2};
        background-color: white;
        width: 200px;
        height: 30px;
        :focus {
          outline-color: ${gray5};
        }
      `}
    />
  </div>
);
