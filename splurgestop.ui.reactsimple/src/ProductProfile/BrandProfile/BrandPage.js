import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from './../../Components/Page';
import { updateBrand } from './BrandCommands';

export function BrandPage({ match }) {
  const [brand, setBrand] = useState(null);
  const [brandsLoading, setBrandsLoading] = useState(true);
  const [isEditing, setEditing] = useState(false);

  useEffect(() => {
    const loadBrand = async () => {
      const id = match.params.id;
      const url = 'https://localhost:44304/api/Brand/' + id;
      const response = await fetch(url);
      const data = await response.json();
      setBrand(data);
      setBrandsLoading(false);
    };

    if (match.params.id) {
      const brandId = match.params.id;
      loadBrand(brandId);
    }
  }, [match.params.id]);

  const editModeClick = () => {
    setEditing(!isEditing);
  };

  const handleSubmit = async () => {
    await updateBrand({
      id: brand.id.value,
      name: brand.name,
    });
  };

  const changeHandler = (e) => {
    brand.name = e.currentTarget.value;
    setBrand(brand);
  };

  return (
    <Page title={brand?.name}>
      <Button onClick={editModeClick} className="float-right">
        Edit
      </Button>
      <div>
        {brandsLoading ? (
          <div
            css={css`
              font-size: 16px;
              font-style: italic;
            `}
          >
            Loading...
          </div>
        ) : (
          <Fragment>
            <div
              css={css`
                margin-top: 5em;
              `}
            >
              {isEditing ? (
                <form onSubmit={handleSubmit}>
                  <input
                    type="text"
                    name="name"
                    label="Brand name"
                    placeholder={brand.name}
                    onChange={changeHandler}
                  />
                  <div
                    css={css`
                      margin: 1em;
                    `}
                  ></div>
                  <input type="submit" value="Save" />
                </form>
              ) : (
                <div>
                  <h1>{brand.name}</h1>
                </div>
              )}
            </div>
          </Fragment>
        )}
      </div>
    </Page>
  );
}
