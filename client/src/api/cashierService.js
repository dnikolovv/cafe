import * as apiClient from "./apiClient";
import { joinUrlWithRoute } from "../utils/urlUtils";

const baseUrl = joinUrlWithRoute(apiClient.BASE_URL, "/cashier/");

export function getCashiers() {
  return apiClient.get(baseUrl);
}
