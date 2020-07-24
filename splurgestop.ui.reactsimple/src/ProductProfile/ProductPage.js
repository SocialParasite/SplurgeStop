import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from './../Components/Page';
import { updateProduct } from './ProductCommands';

export function ProductPage({ match }) {
  const [product, setProduct] = useState(null);
  const [productsLoading, setProductsLoading] = useState(true);
  const [isEditing, setEditing] = useState(false);

  useEffect(() => {
    const loadProduct = async () => {
      const id = match.params.id;
      const url = 'https://localhost:44304/api/Product/' + id;
      const response = await fetch(url);
      const data = await response.json();
      setProduct(data);
      setProductsLoading(false);
    };

    if (match.params.id) {
      const productId = match.params.id;
      loadProduct(productId);
    }
  }, [match.params.id]);

  const editModeClick = () => {
    setEditing(!isEditing);
  };

  const handleSubmit = async () => {
    await updateProduct({
      id: product.id,
      name: product.name,
    });
  };

  return (
    <Page title={product?.name}>
      <Button onClick={editModeClick} className="float-right">
        Edit
      </Button>
      <div>
        {productsLoading ? (
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
                  {/* <div
                    css={css`
                      margin: 1em;
                    `}
                  >
                    {citiesLoading ? (
                      <div
                        css={css`
                          font-size: 16px;
                          font-style: italic;
                        `}
                      >
                        Loading...
                      </div>
                    ) : (
                      <div>
                        <label for="cities">City:</label>
                        <br />
                        <select
                          name="cityId"
                          id="cityId"
                          className="input"
                          type="text"
                          onChange={handleCityChange}
                        >
                          <option>Select city</option>
                          {cities.map((city) => (
                            <option value={city.id} key={city.id}>
                              {city.name}, {city.name}
                            </option>
                          ))}
                        </select>
                      </div>
                    )}
                  </div>
                  <div
                    css={css`
                      margin: 1em;
                    `}
                  >
                    {countriesLoading ? (
                      <div
                        css={css`
                          font-size: 16px;
                          font-style: italic;
                        `}
                      >
                        Loading...
                      </div>
                    ) : (
                      <div>
                        <label for="countries">Country:</label>
                        <br />
                        <select
                          name="countryId"
                          id="countryId"
                          className="input"
                          type="text"
                          onChange={handleCountryChange}
                        >
                          <option>Select country</option>
                          {cities.map((country) => (
                            <option value={country.id} key={country.id}>
                              {country.name}, {country.name}
                            </option>
                          ))}
                        </select>
                      </div>
                    )}
                  </div> */}
                  <input type="submit" value="Save" />
                </form>
              ) : (
                <div>
                  <h2>{product.name}</h2>
                </div>
              )}
            </div>
          </Fragment>
        )}
      </div>
    </Page>
  );
}
