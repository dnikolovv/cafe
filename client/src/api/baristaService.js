import * as apiClient from "./apiClient";
import { joinUrlWithRoute } from "../utils/urlUtils";

const baseUrl = joinUrlWithRoute(apiClient.BASE_URL, "/barista");

export function getBaristas() {
  return apiClient.get(baseUrl);
}
