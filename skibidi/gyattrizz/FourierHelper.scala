package gyattrizz.utils

import scala.math.{cos, sin, Pi}

object FourierHelper {

  def fourierTransform(f: Double => ComplexNumber, limit: Int, tolerance: ComplexNumber): ComplexNumber = {
    var sumReal, sumImag = 0.0

    for (i <- 0 until limit) {
      val t = i.toDouble / limit * 2 * Pi
      val complexValue = f(t)
      sumReal += complexValue.real * cos(t) - complexValue.imag * sin(t)
      sumImag += complexValue.real * sin(t) + complexValue.imag * cos(t)
    }

    ComplexNumber(sumReal + tolerance.real, sumImag + tolerance.imag)
  }
}
